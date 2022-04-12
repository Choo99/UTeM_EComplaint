using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Web.Hosting;
using System.Net.Http.Headers;


namespace WebApi.Controllers
{
    [Authorize]
    public class AduanController : ApiController
    {
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {

                return _userManager ?? System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET api/aduan
        public IEnumerable<string> Get()
        {
           return new string[] { "bbb" };
        }

        // GET api/aduan/5

      


        // POST api/aduan
        public void Post([FromBody]string value)
        {
        }

        // PUT api/aduan/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/aduan/5
        public void Delete(int id)
        {
        }
        [Route("api/Aduan/InserData")]
        [HttpPost]
        public async Task<HttpResponseMessage> InserData()
        {
            var userId = User.Identity.GetUserId(); //requires using Microsoft.AspNet.Identity;
            var user = UserManager.FindById(userId);

            var request = HttpContext.Current.Request;
            string aduan = request.Form["Aduan"];
            string lokasi = request.Form["Lokasi"];
            string telefon = request.Form["Telefon"];
            //HttpResponseMessage result = null;
            //if (request.Files.Count == 0)
            //{
            //    result = Request.CreateResponse(HttpStatusCode.OK, "no1"); ;

            //}
            //var postedFile = request.Files[0]; // I am able to read file here. but i cannot get the Username
            IEnumerable<string> values2 = SQLMigs.HantarIsuuTicket(user.UserName.ToString(), aduan,  lokasi, telefon);
            var myList2 = values2.ToList();
            if (myList2[0] == "ok")
            {
                return Request.CreateResponse(HttpStatusCode.OK, "ok");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, "no2");
            }

        }

        [Route("api/Aduan/{id}/{orderId}/{app_Id}/{mob_Id}/{logincode_Id}/{lat1}/{long1}")]
        public IEnumerable<string> Get(int id, string orderId, string app_Id, string mob_Id, string logincode_Id, string lat1, string long1)
        {
            var userId = User.Identity.GetUserId(); //requires using Microsoft.AspNet.Identity;
            var user = UserManager.FindById(userId);
         
                if (id == 1)
                {
                    // get detailuser and app setting  GetSumbanganDetailPaid(string userid, string sid)
                    return SQLMigs.GetEmergencyButton(user.UserName.ToString());
                }
                if (id == 2)
                {
                    // get detailuser and app setting  GetSumbanganDetailPaid(string userid, string sid)
                    return SQLMigs.GetDetailEmergencyButton(user.UserName.ToString(), orderId, lat1);
                }
                return new string[] { "loginchanged" };
           
        }

    }
}

