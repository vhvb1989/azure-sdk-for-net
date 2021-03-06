// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.Management.Monitor.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Log Analytics destination.
    /// </summary>
    public partial class LogAnalyticsDestination
    {
        /// <summary>
        /// Initializes a new instance of the LogAnalyticsDestination class.
        /// </summary>
        public LogAnalyticsDestination()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the LogAnalyticsDestination class.
        /// </summary>
        /// <param name="workspaceResourceId">The resource ID of the Log
        /// Analytics workspace.</param>
        /// <param name="name">A friendly name for the destination.
        /// This name should be unique across all destinations (regardless of
        /// type) within the data collection rule.</param>
        public LogAnalyticsDestination(string workspaceResourceId, string name)
        {
            WorkspaceResourceId = workspaceResourceId;
            Name = name;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets the resource ID of the Log Analytics workspace.
        /// </summary>
        [JsonProperty(PropertyName = "workspaceResourceId")]
        public string WorkspaceResourceId { get; set; }

        /// <summary>
        /// Gets or sets a friendly name for the destination.
        /// This name should be unique across all destinations (regardless of
        /// type) within the data collection rule.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (WorkspaceResourceId == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "WorkspaceResourceId");
            }
            if (Name == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Name");
            }
        }
    }
}
