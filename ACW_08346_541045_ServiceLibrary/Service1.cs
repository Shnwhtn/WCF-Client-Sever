using System;
using System.Security.Cryptography;
using System.Text;

namespace ACW_08346_541045_ServiceLibrary
{


    public class Service1 : IService1
    {
        // Static Variables
        #region variables
        public static RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
        public static RSAParameters RSAKey = new RSAParameters();

        static SHA1 sha = new SHA1CryptoServiceProvider();
        static SHA256 sha256 = new SHA256CryptoServiceProvider();
        private static string hex;
        private static byte[] signed;
        #endregion  



        #region hello
        public string SayHello(int value)
        {
            string returnValue = "Hello" +"\r\n";
            Console.Write("Client No. " + value + " has contacted the server.\r\n");
            return returnValue;
        }
        #endregion


        #region sortnumbers

        public string SortTheseValues(int[] values)
        {

                int elementCount = 0;
                int[] valueArray = values;
                int length = valueArray.Length;
                Array.Sort(valueArray);
                string outputValues = "Sorted values:\r\n";

            // If 2nd pair put in a space else do not
            for (int i = 0; i < length; i++)
            {
                if (elementCount != 1)
                {
                    outputValues = outputValues + valueArray[i];
                 
                }
                if (elementCount == 1 )
                {
                    outputValues = outputValues + valueArray[i] + " ";
                }

                if ((elementCount == 1 && i + 1 >= length))
                {
                    outputValues = outputValues + valueArray[i];
                }

                if (elementCount == 0)
                {
                    elementCount++;
                }
                else
                {
                    elementCount = 0;
                }
                
            }
                //Server Output
                Console.Write(outputValues);
            //Client Ouput
                return outputValues;
            }

        #endregion


        #region get public keys
        public string GetPubKey()
        {
            // Get public keys
            string exponant = Transform.ByteArrayToHexString(RSAKey.Exponent);
            string modulas = Transform.ByteArrayToHexString(RSAKey.Modulus);
            string fullOutput = exponant + "\n" + modulas;
            //Server Output
            Console.Write("Sending the public key to the client.\r\n");
            //Client Ouput
            return fullOutput;
        }

        #endregion


        #region decrypt message
        public void DecryptMessage(byte[] Encrypted)
        {
            try
            {
                // Get paramaters
                RSA.ImportParameters(RSAKey);
                //Use private key
                byte[] decryptedData = RSA.Decrypt(Encrypted, true);
                //Server Ouput
                Console.Write("Decrypted message is: " + (System.Text.Encoding.ASCII.GetString(decryptedData)) + ".\r\n");
            }
            catch(CryptographicException e)
            {
                Console.Write(e.Message);
            }
        }
        #endregion

        #region sha1 hash

        public string Sha1Hash(string toHash)
        {
            hex = "";
            try
            {

                byte[] data = System.Text.Encoding.ASCII.GetBytes(toHash);
                byte[] hashed = sha.ComputeHash(data);
                hex = Transform.ByteArrayToHexString(hashed);
                //Server Output
                Console.Write("SHA-1 hash of " + toHash + " is " + hex + ".\r\n");

            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
            }
            //Client Output
            return hex;
        }

        #endregion


        #region sha256

        public string Sha256Hash(string toHash)
        {
            hex = "";
            try
            {
                byte[] data = Encoding.ASCII.GetBytes(toHash);
                byte[] hashed;

                hashed = sha256.ComputeHash(data);
                hex = Transform.ByteArrayToHexString(hashed);
                //Server output
                Console.Write("SHA-256 hash of " + toHash + " is " + hex + ".\r\n");

            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
            }
            //Client Output
            return hex;
        }
        #endregion


        #region signed data
        public byte[] SignData(string signedData)
        {
            // If string when turned to upper
            if (signedData.ToUpper() == "CHEESECAKE")
            {
                // Server Output
                Console.Write("No cheesecake allowed.\r\n");
                byte[] signed = System.Text.Encoding.ASCII.GetBytes(" ");
            }
            else
            {
                try
                {
                   
                    // Import stuff
                    RSA.ImportParameters(RSAKey);
                    // Export Private Key
                    RSAParameters RSAPrivKey = RSA.ExportParameters(true);

                    ASCIIEncoding ByteConverter = new ASCIIEncoding();
                    byte[] asciiByteMessage = ByteConverter.GetBytes(signedData);
                    // Server Output
                    Console.Write("Signing data: " + signedData + ".\r\n");
                    signed = HashAndSignBytes(asciiByteMessage, RSAPrivKey);
                }
                catch (CryptographicException e)
                {
                    Console.Write(e.Message);
                }
            }
            return signed;
        }
    

        // This is the hash and sign bytes function
        public static byte[] HashAndSignBytes(byte[] DataToSign, RSAParameters Key)
        {
            try
            {


                RSA.ImportParameters(Key);
                // Hash and sign the data. Pass a new instance of SHA1CryptoServiceProvider
                // to specify the use of SHA1 for hashing.
                return RSA.SignData(DataToSign, new SHA1CryptoServiceProvider());
            }
            catch (CryptographicException e)
            {
                Console.Write(e.Message);

                return null;
            }
        }
        #endregion
    }
}
