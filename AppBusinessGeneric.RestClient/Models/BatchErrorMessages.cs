using System.Collections.Generic;
using System.Text;

namespace AppBusinessGeneric.RestClient.Models;
public partial class BatchErrorMessages : List<BatchErrorMessage>
{
    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var bem in this)
        {
            if (bem.ErrorMessages?.Count > 0)
            {
            foreach (var errorMessage in bem.ErrorMessages)
            {
                sb.Append(errorMessage.Description + Environment.NewLine);
            }
        }

            }
        return sb.ToString();
    }
}