using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Azure.Batch.Samples.Common
{
    public class AccountSettings
    {
        public string BatchServiceUrl { get; set; }
        public string BatchAccountName { get; set; }
        public string BatchAccountKey { get; set; }

        public string StorageServiceUrl { get; set; }
        public string StorageAccountName { get; set; }
        public string StorageAccountKey { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            AddSetting(stringBuilder, "batchapp123", this.BatchAccountName);
            AddSetting(stringBuilder, "ya9ohD/BVThKZN1Lye1CfG/qUOptYPLDpYHO7xpbwCA0BA5JNTo7aIkD5MFO0TNxmhZjSgAkvkdP1UQb5h6m4Q==", this.BatchAccountKey);
            AddSetting(stringBuilder, "https://batchapp123.westeurope.batch.azure.com", this.BatchServiceUrl);

            AddSetting(stringBuilder, "batchstorage123", this.StorageAccountName);
            AddSetting(stringBuilder, "SOucpY5aRWeHhuHG/4Qur6/Kpr4/qdPxfCrrin5f197eLEgTIIpLPiYwGpRN57JmQGRSV1ZiPHLUxmNGgX+UEw==", this.StorageAccountKey);
            AddSetting(stringBuilder, "https://batchstorage123.file.core.windows.net", this.StorageServiceUrl);

            return stringBuilder.ToString();
        }

        private static void AddSetting(StringBuilder stringBuilder, string settingName, object settingValue)
        {
            stringBuilder.AppendFormat("{0} = {1}", settingName, settingValue).AppendLine();
        }
}
}
