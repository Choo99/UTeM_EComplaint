using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Text;

namespace WebApi
{
    public class DigitalSignature
    {
        private RSAParameters publicKey;
        private RSAParameters privateKey;

        public void AssignNewKey()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                publicKey = rsa.ExportParameters(false);
                privateKey = rsa.ExportParameters(true);
            }
        }

        public byte[] SignData(byte[] hashOfDataToSign)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.ImportParameters(privateKey);

                var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
                rsaFormatter.SetHashAlgorithm("SHA256");

                return rsaFormatter.CreateSignature(hashOfDataToSign);
            }
        }

        public bool VerifySignature(byte[] hashOfDataToSign, byte[] signature)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.ImportParameters(publicKey);

                var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
                rsaDeformatter.SetHashAlgorithm("SHA256");

                return rsaDeformatter.VerifySignature(hashOfDataToSign, signature);
            }
        }
    }
}




//namespace WebApplication1
//{
//    public partial class WebForm1 : System.Web.UI.Page
//    {
//        protected void Page_Load(object sender, EventArgs e)
//        {
//            var document = Encoding.UTF8.GetBytes("Document to Sign");
//            byte[] hashedDocument;

//            using (var sha256 = SHA256.Create())
//            {
//                hashedDocument = sha256.ComputeHash(document);
//            }

//            var digitalSignature = new DigitalSignature();
//            digitalSignature.AssignNewKey();

//            var signature = digitalSignature.SignData(hashedDocument);




//            var verified = digitalSignature.VerifySignature(hashedDocument, signature);


//            TextBox2.Text = System.Text.Encoding.Default.GetString(document);

//            TextBox3.Text = Convert.ToBase64String(signature);


//            if (verified)
//            {
//                TextBox4.Text = "The digital signature has been correctly verified.";
//            }
//            else
//            {
//                TextBox4.Text = "The digital signature has NOT been correctly verified.";
//            }


//        }


//    }



    