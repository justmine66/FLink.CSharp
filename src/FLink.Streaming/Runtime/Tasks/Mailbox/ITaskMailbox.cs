using System.Collections.Generic;
using FLink.Core.Api.CSharp.Threading;
using FLink.Core.Exceptions;

namespace FLink.Streaming.Runtime.Tasks.Mailbox
{
    /// <summary>
    /// A task mailbox provides read and write access to a mailbox and has a lifecycle of open -> (quiesced) -> closed.
    /// Mails have a priority that can be used to retrieve only relevant letters.
    ///
    /// Threading model
    ///
    /// Life cycle
    /// In the open state, the mailbox supports put and take operations. In the quiesced state, the mailbox supports only take operations.
    ///
    /// Batch
    /// A batch is a local view on the mailbox that does not contain simultaneously added mails similar to iterators of copy-on-write collections.
    ///
    /// A batch serves two purposes: it reduces synchronization if more than one mail is processable at the time of the creation of a batch. Furthermore, it allows to divide the work of a mailbox in smaller logical chunks, such that the task threads cannot be blocked by a mail that enqueues itself and thus provides input starvation.
    /// </summary>
    public interface ITaskMailbox
    {
        /// <summary>
        /// The minimal priority for mails.
        /// The priority is used when no operator is associated with the mail.
        /// </summary>
        int MinPriority { get; }

        /// <summary>
        /// The maximal priority for mails.
        /// This priority indicates that the message should be performed before any mail associated with an operator.
        /// </summary>
        int MaxPriority { get; }

        /// <summary>
        /// Check if the current thread is the mailbox thread.
        /// Read operations will fail if they are called from another thread.
        /// </summary>
        bool IsMailboxThread { get; }

        /// <summary>
        /// Returns true if the mailbox contains mail.
        /// Must be called from the mailbox thread(<see cref="IsMailboxThread"/>).
        /// </summary>
        bool HasMail { get; }

        /// <summary>
        /// Returns an optional with either the oldest mail from the mailbox (head of queue) if the mailbox is not empty or an empty optional otherwise.
        /// Must be called from the mailbox thread(<see cref="IsMailboxThread"/>). 
        /// </summary>
        /// <param name="priority"></param>
        /// <returns>an optional with either the oldest mail from the mailbox (head of queue) if the mailbox is not empty or an empty optional otherwise.</returns>
        /// <exception cref="IllegalStateException">if mailbox is already closed.</exception>
        Mail TryTake(int priority);

        /// <summary>
        /// This method returns the oldest mail from the mailbox (head of queue) or blocks until a mail is available.
        /// Must be called from the mailbox thread(<see cref="IsMailboxThread"/>). 
        /// </summary>
        /// <param name="priority"></param>
        /// <returns>the oldest mail from the mailbox (head of queue).</returns>
        /// <exception cref="InterruptedException">on interruption.</exception>
        /// <exception cref="IllegalStateException">if mailbox is already closed.</exception>
        Mail Take(int priority);

        #region [ Batch ]

        /// <summary>
        /// true if there is at least one element in the batch; that is, if there is any mail at all at the time of the invocation.
        /// Creates a batch of mails that can be taken with <see cref="TryTakeFromBatch"/>.
        /// The batch does not affect <see cref="TryTake"/> and <see cref="Take"/>; that is, they return the same mails even if no batch would have been created.
        /// The default batch is empty. Thus, this method must be invoked once before <see cref="TryTakeFromBatch"/>.
        /// Must be called from the mailbox thread(<see cref="IsMailboxThread"/>).
        /// </summary>
        bool CreateBatch { get; }

        /// <summary>
        /// Returns an optional with either the oldest mail from the batch (head of queue) if the batch is not empty or an empty optional otherwise.
        /// Must be called from the mailbox thread(<see cref="IsMailboxThread"/>). 
        /// </summary>
        /// <returns>an optional with either the oldest mail from the batch (head of queue) if the batch is not empty or an empty optional otherwise.</returns>
        /// <exception cref="IllegalStateException">if mailbox is already closed.</exception>
        Mail TryTakeFromBatch();

        #endregion

        #region [ Write methods ]

        /// <summary>
        /// Enqueues the given mail to the mailbox and blocks until there is capacity for a successful put.
        /// Mails can be added from any thread.
        /// </summary>
        /// <param name="mail">the mail to enqueue.</param>
        /// <exception cref="IllegalStateException">if mailbox is already closed.</exception>
        void Put(Mail mail);

        /// <summary>
        /// Adds the given action to the head of the mailbox.
        /// Mails can be added from any thread.
        /// </summary>
        /// <param name="mail">the mail to enqueue.</param>
        /// <exception cref="IllegalStateException">if mailbox is already closed.</exception>
        void PutFirst(Mail mail);

        #endregion

        #region [ Lifecycle methods ]

        /// <summary>
        /// Drains the mailbox and returns all mails that were still enqueued.
        /// </summary>
        /// <returns>list with all mails that where enqueued in the mailbox.</returns>
        IList<Mail> Drain();

        /// <summary>
        /// Quiesce the mailbox. In this state, the mailbox supports only take operations and all pending and future put operations will throw <see cref="IllegalStateException"/>.
        /// </summary>
        void Quiesce();

        /// <summary>
        /// Close the mailbox. In this state, all pending and future put operations and all pending and future take operations will throw <see cref="IllegalStateException"/>. Returns all mails that were still enqueued.
        /// </summary>
        /// <returns>list with all mails that where enqueued in the mailbox at the time of closing.</returns>
        List<Mail> Close();

        /// <summary>
        /// Returns the current state of the mailbox as defined by the lifecycle enum <see cref="MailboxLifecycle"/>.
        /// </summary>
        MailboxLifecycle Lifecycle { get; }

        /// <summary>
        /// Runs the given code exclusively on this mailbox. No synchronized operations can be run concurrently to the given runnable.
        /// Use this methods when you want to atomically execute code that uses different methods (e.g., check for state and then put message if open). 
        /// </summary>
        /// <param name="runnable">the runnable to execute</param>
        void RunExclusively(IRunnable runnable); 

        #endregion
    }

    public enum MailboxLifecycle
    {
        Open, Quiesced, Closed
    }
}
