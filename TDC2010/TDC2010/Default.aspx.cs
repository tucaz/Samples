using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TDC2010.Core.Dao;
using TDC2010.Core.Domain;
using System.Web.UI.HtmlControls;

namespace TDC2010
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {            
            ProductDao myDao = new ProductDao();
            List<Product> allProducts = myDao.GetAllProducts();

            foreach (Product myProduct in allProducts)
            {
                HtmlGenericControl li = new HtmlGenericControl("li");

                HyperLink productLink = new HyperLink();
                productLink.NavigateUrl = "ProductDetail.aspx?productId=" + myProduct.Id.ToString();
                productLink.Text = myProduct.Name;

                li.Controls.Add(productLink);
                productList.Controls.Add(li);
            }
        }
    }
}
