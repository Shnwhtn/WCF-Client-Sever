using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Security.Cryptography;

namespace ACW_08346_541045_ServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        string SayHello(int value);

        [OperationContract]
        string GetPubKey();


        [OperationContract]
        string SortTheseValues(int[] values);

        [OperationContract]
        //void DecryptMessage(string Encrypted);
        void DecryptMessage(byte[] Encrypted);

        [OperationContract]
        string Sha1Hash(string toHash);

        [OperationContract]
        string Sha256Hash(string toHash);

        [OperationContract]
        byte[] SignData(string signedData);

    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]


    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }

        [DataMember]
        public byte[] byteArray;
    }
}
