using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nop.Web.Models.ShoppingCart
{
    public class StartOrderModel
    {
        public StartOrderModel()
        {
            this.LiveScenes = new List<LiveSceneModel>();
        }

        public bool CustomerIsRegistered { get; set; }

        public string Name { get; set; }
        public string Gender { get; set; }
        public string WeiXinName { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string ConfirmedPassword { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string AddressDetail { get; set; }

        public int OrderStep { get; set; }
        //public ContactInfoModel ContactInfo { get; set; }

        public string CustomerNote { get; set; }

        public ShoppingCartModel ShoppingCart { get; set; }

        public List<LiveSceneModel> LiveScenes { get; set; }

        #region

        public class LiveSceneModel
        {
            public int OrderItemId { get; set; }
            public string Room { get; set; }
            public string Window { get; set; }
        }

        public class ContactInfoModel
        {
            public string Name { get; set; }
            public string Sex { get; set; }
            public string WeiXinName { get; set; }
            public string Phone { get; set; }
            public string Password { get; set; }
            public string ConfirmedPassword { get; set; }
            public string City { get; set; }
            public string District { get; set; }
            public string AddressDetail { get; set; }
        }

        #endregion

    }
}