using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace ACW_08346_541045_Client
{
    class Program
    {

        #region Variables
        public static string getPubKey = "";
        public static string[] details;
        static RSACryptoServiceProvider RSAClient = new RSACryptoServiceProvider();
        static RSAParameters RSAKeyClient = new RSAParameters();
        private static string Sha256Message = "";
        private static string Sha1Message = "";
        public static string MessageToEncode;
        private static byte[] hash;
        private static byte[] byteMessage;
        private static string clientInput;
        private static string command;

        #endregion

        #region Verifyahash
        // Function to verify a hash
        public static bool VerifySignedHash(byte[] DataToVerify, byte[] SignedData, RSAParameters Key)
        {
            try
            {
                // Create a new instance of RSACryptoServiceProvider using the 
                // key from RSAParameters.
                RSACryptoServiceProvider RSAClient = new RSACryptoServiceProvider();

                RSAClient.ImportParameters(Key);

                // Verify the data using the signature.  Pass a new instance of SHA1CryptoServiceProvider
                // to specify the use of SHA1 for hashing.
                return RSAClient.VerifyData(DataToVerify, new SHA1CryptoServiceProvider(), SignedData);

            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return false;
            }
        }
        #endregion

        static void Main(string[] args)
        {
            // Service Reference
            ServiceReference1.Service1Client WCFLib = new ACW_08346_541045_Client.ServiceReference1.Service1Client();

            // Get first line of how many commands then parse into an init
            string howManyCommands = Console.ReadLine();
            int intManyCommands;
            int.TryParse(howManyCommands, out intManyCommands);

            // Create a string array and size it with how mnay commands
            string[] clientCommands = new string[intManyCommands];
            // for each line
            for (int i = 0; i < intManyCommands; i++)
            {
                // store the line in this array
                clientCommands[i] = Console.ReadLine();
            }

            // For each command
            for (int i = 0; i < intManyCommands; i++)
            {
                // Create a string from the array
                string c = clientCommands[i].ToString();
                // create a substring, this is the the second half of the string
                string d = c.Substring(c.IndexOf(' ') + 1);
                // This is the command bit ( sha1, hello, pubkey etc etc)
                string[] inputCommands = c.Split(' ');
                // Format to lower case
                command = inputCommands[0].ToLower();

                // use the command for the relevent function

                switch (command)
                {
                    #region hello
                    case "hello":
                        // Parsed holder
                        int parsed;
         
                            if (Int32.TryParse(d, out parsed))
                            {
                                Console.Write(WCFLib.SayHello(parsed));
                            }

                        break;
                    #endregion
                    #region pubkey
                    case "pubkey":
                        {
                            // Get Pubkey from server
                            getPubKey = WCFLib.GetPubKey();
                            // Split strings 
                            details = getPubKey.Split('\n');
                            // Store details
                            RSAKeyClient.Exponent = Transform.StringToByteArray(details[0]);
                            RSAKeyClient.Modulus = Transform.StringToByteArray(details[1]);
                            //client output
                            Console.WriteLine(details[0]);
                            Console.WriteLine(details[1]);
                        }
                        break;
                    #endregion

                    #region sort
                    case "sort":
                        // List to store int elemenets
                        List<int> valueList = new List<int>();
                        // Split the number of elements, the first number
                        string[] valueElements = d.Split(' ');
                        // for parsing the above string
                        int numOfElements;
                        // get a string of all the other values
                        string elements = d.Substring(d.IndexOf(' ') + 1);
                        // Create a string array of all the values and removes spaces
                        string[] seperators = {" "};
                        string[] numbers = elements.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                        // Parse all numbers and put them in the vlaue list
                            if (Int32.TryParse(valueElements[0], out numOfElements))
                            {
                                    for (int x = 0; x < numOfElements; x++)
                                    {
                                        int element = 0;
                                        if (Int32.TryParse(numbers[x], out element))
                                        {
                                            valueList.Add(element);
                                        }
                                    }
                                    // List to array
                                    int[] valueArray = valueList.ToArray();
                                    // Send to server then output
                                    Console.Write(WCFLib.SortTheseValues(valueArray));
                                    // Clear the array
                                    Array.Clear(valueArray, 0, valueArray.Length);
                                    valueList.Clear();
                            }
                        break;
                    #endregion

                    #region Encoding
                    case "enc":
                        if (getPubKey == "")
                        {
                            // If no pubkey is obtainted
                            Console.Write("No public key.\r\n");
                        }
                        else
                        {
                            // From earlier array
                                string MessageToEncode = d;
                                try
                                {

                                //empty byte
                                    byte[] encryptedData;
                                //import paramters
                                    RSAClient.ImportParameters(RSAKeyClient);
                                // turn message into byte
                                    byte[] DataToEncrypt = System.Text.Encoding.ASCII.GetBytes(MessageToEncode);
                                //
                                    encryptedData = RSAClient.Encrypt(DataToEncrypt, false);
                                    Console.Write("Encrypted message sent.\r\n");
                                    WCFLib.DecryptMessage(encryptedData);
                                    MessageToEncode = "";
                                }
                                catch (CryptographicException e)
                                {
                                    Console.Write(e.Message);
                                }
                            }
                            #endregion
                        break;

                    #region sha1
                    case "sha1":
                        {

                            Sha1Message = d;
                            Console.WriteLine(WCFLib.Sha1Hash(Sha1Message));
                            Sha1Message = "";
                            }
                            break;


                    #region sha256
                    case "sha256":
                        { 
                             Sha256Message = d;
                        Console.WriteLine(WCFLib.Sha256Hash(Sha256Message));
                            Sha256Message = "";
                        }
                        break;
                    #endregion

                    #region signing
                    case "sign":
                        if (getPubKey == "")
                        {
                            Console.Write("No public key.\r\n");
                        }
                        else
                        {
                        string MessageToEncode = d;
                                try
                                {
                                    byte[] signedMessage = WCFLib.SignData(MessageToEncode);
                                    ASCIIEncoding ByteConverter = new ASCIIEncoding();
                                    byte[] originalMessage = ByteConverter.GetBytes(MessageToEncode);
                                    RSAClient.ImportParameters(RSAKeyClient);


                                    if (VerifySignedHash(originalMessage, signedMessage, RSAKeyClient))
                                    {
                                        Console.Write("Signature successfully verified.\r\n");
                                    }
                                    else
                                    {
                                        Console.Write("Data not signed.\r\n");
                                    }
                                }
                                catch (CryptographicException e)
                                {
                                    Console.Write(e.Message);
                                }
                            
                        }
                        break;
                        #endregion
                }
            }

        }

    }
}

#endregion



