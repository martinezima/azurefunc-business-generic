using System.Text;

namespace AppBusinessGeneric.RestClient.Models;

public partial class ErrorMessages : List<ErrorMessage>
    {
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("Validation Errors : ");
            foreach (var errorMessage in this)
            {
                sb.AppendLine(errorMessage.ToString());
            }
            return sb.ToString();
        }
    }