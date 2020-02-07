﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Globalization;
using Azure.Messaging.ServiceBus.Receiver;

namespace Azure.Messaging.ServiceBus
{
    /// <summary>
    ///   Serves as a basis for exceptions produced within the Event Hubs
    ///   context.
    /// </summary>
    ///
    /// <seealso cref="System.Exception" />
    ///
    public class ServiceBusException : Exception
    {
        /// <summary>
        ///   Indicates whether an exception should be considered transient or final.
        /// </summary>
        ///
        /// <value><c>true</c> if the exception is likely transient; otherwise, <c>false</c>.</value>
        ///
        public bool IsTransient { get; }

        /// <summary>
        ///   The reason for the failure of an Event Hubs operation that resulted
        ///   in the exception.
        /// </summary>
        ///
        public FailureReason Reason { get; }

        /// <summary>
        ///   The name of the Event Hubs to which the exception is associated.
        /// </summary>
        ///
        /// <value>The name of the Event Hub, if available; otherwise, <c>null</c>.</value>
        ///
        public string EntityName { get; }

        /// <summary>
        ///   Gets a message that describes the current exception.
        /// </summary>
        ///
        public override string Message
        {
            get
            {
                if (string.IsNullOrEmpty(EntityName))
                {
                    return base.Message;
                }

                return string.Format(CultureInfo.InvariantCulture, "{0} ({1})", base.Message, EntityName);
            }
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ServiceBusException"/> class.
        /// </summary>
        ///
        /// <param name="isTransient"><c>true</c> if the exception should be considered transient; otherwise, <c>false</c>.</param>
        /// <param name="entityName">The name of the Event Hub to which the exception is associated.</param>
        ///
        public ServiceBusException(bool isTransient,
                                  string entityName) : this(isTransient, entityName, null, FailureReason.GeneralError, null)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ServiceBusException"/> class.
        /// </summary>
        ///
        /// <param name="isTransient"><c>true</c> if the exception should be considered transient; otherwise, <c>false</c>.</param>
        /// <param name="eventHubName">The name of the Event Hub to which the exception is associated.</param>
        /// <param name="reason">The reason for the failure that resulted in the exception.</param>
        ///
        public ServiceBusException(bool isTransient,
                                  string eventHubName,
                                  FailureReason reason) : this(isTransient, eventHubName, null, reason, null)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ServiceBusException"/> class.
        /// </summary>
        ///
        /// <param name="isTransient"><c>true</c> if the exception should be considered transient; otherwise, <c>false</c>.</param>
        /// <param name="entityName">The name of the Event Hub to which the exception is associated.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        ///
        public ServiceBusException(bool isTransient,
                                  string entityName,
                                  string message) : this(isTransient, entityName, message, FailureReason.GeneralError, null)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ServiceBusException"/> class.
        /// </summary>
        ///
        /// <param name="isTransient"><c>true</c> if the exception should be considered transient; otherwise, <c>false</c>.</param>
        /// <param name="entityName">The name of the Event Hub to which the exception is associated.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="reason">The reason for the failure that resulted in the exception.</param>
        ///
        public ServiceBusException(bool isTransient,
                                  string entityName,
                                  string message,
                                  FailureReason reason) : this(isTransient, entityName, message, reason, null)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ServiceBusException"/> class, using the <paramref name="reason"/>
        ///   to detect whether or not it should be transient.
        /// </summary>
        ///
        /// <param name="eventHubName">The name of the Event Hub to which the exception is associated.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="reason">The reason for the failure that resulted in the exception.</param>
        ///
        public ServiceBusException(string eventHubName,
                                  string message,
                                  FailureReason reason) : this(default, eventHubName, message, reason, null)
        {
            switch (reason)
            {
                case FailureReason.ServiceCommunicationProblem:
                case FailureReason.ServiceTimeout:
                case FailureReason.ServiceBusy:
                    IsTransient = true;
                    break;

                default:
                    IsTransient = false;
                    break;
            }
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ServiceBusException"/> class.
        /// </summary>
        ///
        /// <param name="isTransient"><c>true</c> if the exception should be considered transient; otherwise, <c>false</c>.</param>
        /// <param name="entityName">The name of the Event Hub to which the exception is associated.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        ///
        public ServiceBusException(bool isTransient,
                                  string entityName,
                                  string message,
                                  Exception innerException) : this(isTransient, entityName, message, FailureReason.GeneralError, innerException)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ServiceBusException"/> class.
        /// </summary>
        ///
        /// <param name="isTransient"><c>true</c> if the exception should be considered transient; otherwise, <c>false</c>.</param>
        /// <param name="eventHubName">The name of the Event Hub to which the exception is associated.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="reason">The reason for the failure that resulted in the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        ///
        public ServiceBusException(bool isTransient,
                                  string eventHubName,
                                  string message,
                                  FailureReason reason,
                                  Exception innerException) : base(message, innerException)
        {
            IsTransient = isTransient;
            EntityName = eventHubName;
            Reason = reason;
        }

        /// <summary>
        ///   The set of well-known reasons for an Event Hubs operation failure that
        ///   was the cause of an exception.
        /// </summary>
        ///
        public enum FailureReason
        {
            /// <summary>The exception was the result of a general error within the client library.</summary>
            GeneralError,

            /// <summary>An operation has been attempted using an Event Hubs client instance which has already been closed.</summary>
            ClientClosed,

            /// <summary>A client was forcefully disconnected from an Event Hub instance.</summary>
            ConsumerDisconnected,

            /// <summary>An Event Hubs resource, such as an Event Hub, consumer group, or partition cannot be found by the Event Hubs service.</summary>
            ResourceNotFound,

            /// <summary>A message is larger than the maximum size allowed for its transport.</summary>
            MessageSizeExceeded,

            /// <summary>The quota applied to an Event Hubs resource has been exceeded while interacting with the Azure Event Hubs service.</summary>
            QuotaExceeded,

            /// <summary>The Azure Event Hubs service reports that it is busy in response to a client request to perform an operation.</summary>
            ServiceBusy,

            /// <summary>An operation or other request timed out while interacting with the Azure Event Hubs service.</summary>
            ServiceTimeout,

            /// <summary>There was a general communications error encountered when interacting with the Azure Event Hubs service.</summary>
            ServiceCommunicationProblem
        }
    }
}
