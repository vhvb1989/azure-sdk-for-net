// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace Azure.ResourceManager.MachineLearningServices.Models
{
    /// <summary> The AccountKeySection. </summary>
    public partial class AccountKeySection
    {
        /// <summary> Initializes a new instance of AccountKeySection. </summary>
        public AccountKeySection()
        {
        }

        /// <summary> Initializes a new instance of AccountKeySection. </summary>
        /// <param name="key"> Storage account key. </param>
        internal AccountKeySection(string key)
        {
            Key = key;
        }

        /// <summary> Storage account key. </summary>
        public string Key { get; set; }
    }
}
