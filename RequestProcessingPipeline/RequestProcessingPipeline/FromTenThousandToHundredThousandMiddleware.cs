namespace RequestProcessingPipeline
{
    public class FromTenThousandToHundredThousandMiddleware
    {
        private readonly RequestDelegate _next;

        public FromTenThousandToHundredThousandMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            int number;
            try
            {
                string? remainderNumber = context.Session.GetString("remainder_number");
                if (!string.IsNullOrEmpty(remainderNumber) && int.TryParse(remainderNumber, out int parsedNumber))
                {
                    number = parsedNumber;
                }
                else
                {
                    string? token = context.Request.Query["number"];
                    if (string.IsNullOrEmpty(token))
                    {
                        await context.Response.WriteAsync("Incorrect parameter");
                        return;
                    }
                    number = Math.Abs(Convert.ToInt32(token));
                }

                if (number < 10000)
                {
                    await _next.Invoke(context);
                    return;
                }
                if (number >= 100000)
                {
                    await context.Response.WriteAsync("Number greater than 100000");
                    return;
                }

                string[] Ones = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
                string[] Tens = { "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
                string[] Teens = { "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };

                int numTenThousand = number / 1000;
                int remainder = number % 1000;
                string tenThousandText = "";

                if (numTenThousand == 10)
                {
                    tenThousandText = "ten";
                }
                else if (numTenThousand >= 11 && numTenThousand <= 19)
                {
                    tenThousandText = Teens[numTenThousand - 11];
                }
                else if (numTenThousand >= 20)
                {
                    tenThousandText = Tens[(numTenThousand / 10) - 2] + (numTenThousand % 10 > 0 ? " " + Ones[numTenThousand % 10 - 1] : "");
                }
                else
                {
                    tenThousandText = Ones[numTenThousand - 1];
                }
                tenThousandText += " thousand";

                if (remainder == 0)
                {
                    await context.Response.WriteAsync("Your number is " + tenThousandText);
                }
                else
                {
                    context.Session.SetString("remainder_number", remainder.ToString());
                    await _next.Invoke(context);
                    string? remainderText = context.Session.GetString("remainder_text");
                    if (!string.IsNullOrEmpty(remainderText))
                    {
                        tenThousandText += " " + remainderText;
                        await context.Response.WriteAsync("Your number is " + tenThousandText);
                    }
                    context.Session.Remove("remainder_number");
                }
            }
            catch (Exception)
            {
                await context.Response.WriteAsync("Incorrect parameter");
            }
        }
    }
}