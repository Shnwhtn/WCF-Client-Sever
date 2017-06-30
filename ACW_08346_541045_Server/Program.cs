using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Added by the reference
using System.ServiceModel;
using ACW_08346_541045_ServiceLibrary;
using System.Security.Cryptography;
using System.ServiceModel.Description;
using System.Threading;

namespace ACW_08346_541045_Server
{

    class Program : Service1
    {
        
        
        // Launch connections in background thread
        static void Connections()
        {
            ServiceHost appServiceHost = new ServiceHost(typeof(Service1));
            appServiceHost.Open();
        }

        // 9 Second Count Down
        static void timer()
        {
            Thread.Sleep(9000);
            Environment.Exit(0);
        }


        //Main
        static void Main(string[] args)
        { 
            //String array for the 8 line input
            string[] inputarray = new string[8];
            // Counter
            int count = 0;
            // True bool, keeps window active
            bool active = true;


            // Thread for connections
            Thread t = new Thread(Connections);
            // Thread for timer
            Thread tp = new Thread(timer);
            // Start connections and print server running
            t.Start();
            Console.WriteLine("Server running...");


            // For Debug
            /* 
            inputarray[0] ="0b537c55d90abc38f36c59d3b12b142c097e261999394a3ff6116941383cbf1da4e0634f7a95435dafbac669a005d337b4a7d97084ca93ddd6acd9ec735290996ae7bdac85c84d8b1f1e24c012bb844793f1acf5805d09ba421c4a8f58124f60754dcf615eda671c0d86233ff51656f141e05b89ee6302f30b8dc3bcd5bb2d41";
            inputarray[1]= "31375be61215c75f69696db32ba6786723b7064b0ad69361ce8850fbbed2c0620bf99352ea93cffec867231bc19d92a3ae3b8d86ba0e88b5133a6321ddc7ef41";
            inputarray[2] ="265bcda234a4d4d6887986728a1512a49bdeda70eaad50c59265ebd8e4d883aa6376727382f944734c75b3ce5ec1680bfe380da022f0f32e4f1b383740122497";
            inputarray[3] = "010001";
            inputarray[4] = "c64f02ba0562c196f9e528c55b4056cfdd06f240b1e21edee98d0c833eb08bfd8b2678f42402c32007f172abcd7b662fac423d0fcf1b56dc9b55a2a3247b4706";
            inputarray[5]="92644d841f0775d941fa5ba35aa3ccf65d7680135a719f47070fd8256fc0f54a7ead22191653f58be1d4d6ffa4b091af5d342f2cab1bed8ad82ca96722688e955e42b384261d1ccdf138e6d3334969342db1a7005e3e0e26203eb72758d8161619ccd8ca71217234fac92d41e82d9b35a9a29c3dbd1d4c6e3496330c96aa69db";
            inputarray[6]="c92d8a37c1b9dd2dca44ad17b3625fbb78a241e9c28d78c3d9c6dcb2a2ef96d25b1a207c205c650465b57d67559ee471e41bf86fa294b291c73e94a84eb6e741";
            inputarray[7]="ba48ce81e9684e5f375fdf72df92abb5d64b33f455f03e947181bd8b6c70b101d2d44f7bc4ad3917684252f9e223125edae0697a0b70c76601cfeacceb54861b";
            */
            tp.Start();
            while (active)
            {

                // Readlines
                string input = Console.ReadLine();
                // If not empty strings provided or the count is 8
                if (!string.IsNullOrEmpty(input) && !(count == 8))
                {
                    inputarray[count] = input;
                    count++;
                }
                
                // When 8 strings are provided , this will transfer the hex strings to byte then put in the rsa paramaters
                if (count == 8)
                {
                    RSAKey.D = Transform.StringToByteArray(inputarray[0]);
                    RSAKey.DP = Transform.StringToByteArray(inputarray[1]);
                    RSAKey.DQ = Transform.StringToByteArray(inputarray[2]);
                    RSAKey.Exponent = Transform.StringToByteArray(inputarray[3]);
                    RSAKey.InverseQ = Transform.StringToByteArray(inputarray[4]);
                    RSAKey.Modulus = Transform.StringToByteArray(inputarray[5]);
                    RSAKey.P = Transform.StringToByteArray(inputarray[6]);
                    RSAKey.Q = Transform.StringToByteArray(inputarray[7]);
                }
                

            }
            
        }
    }

}