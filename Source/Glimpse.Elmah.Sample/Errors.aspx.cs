using System;
using System.Linq;
using System.Reflection;
using System.Web.UI;

namespace Glimpse.Elmah.Sample
{
    public partial class Errors : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            var exceptionType = (typeof(Exception));
            var exceptionTypes = Assembly
                .GetAssembly(exceptionType)
                .GetTypes()
                .Where(t => t.IsSubclassOf(exceptionType))
                .ToList();

            ExceptionTypeToThrowDropDownList.DataValueField = "FullName";
            ExceptionTypeToThrowDropDownList.DataTextField = "Name";
            ExceptionTypeToThrowDropDownList.DataSource = exceptionTypes;
            ExceptionTypeToThrowDropDownList.DataBind();
        }

        public void SubmitButtonClick(object sender, EventArgs e)
        {
            var exceptionType = Type.GetType(ExceptionTypeToThrowDropDownList.SelectedValue);
            if (exceptionType == null)
                return;

            var exception = (Exception)Activator.CreateInstance(exceptionType);
            throw exception;
        }
    }
}