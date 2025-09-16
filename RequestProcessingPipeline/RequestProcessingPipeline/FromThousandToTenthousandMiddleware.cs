namespace RequestProcessingPipeline
{
    public class FromThousandToTenthousandMiddleware
    {
        private readonly RequestDelegate _next;

        public FromThousandToTenthousandMiddleware(RequestDelegate next)
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

                if (number < 1000 || number >= 10000)
                {
                    await _next.Invoke(context);
                    return;
                }

                string[] Ones = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

                int numThousand = number / 1000;
                int remainder = number % 1000;
                string thousandText = Ones[numThousand - 1] + " thousand";

                if (remainder == 0)
                {
                    if (string.IsNullOrEmpty(context.Session.GetString("remainder_number")))
                    {
                        await context.Response.WriteAsync("Your number is " + thousandText);
                    }
                    else
                    {
                        context.Session.SetString("remainder_text", thousandText);
                    }
                }
                else
                {
                    context.Session.SetString("remainder_number", remainder.ToString());
                    await _next.Invoke(context);
                    string? remainderText = context.Session.GetString("remainder_text");
                    if (!string.IsNullOrEmpty(remainderText))
                    {
                        thousandText += " " + remainderText;
                    }

                    context.Session.Remove("remainder_number");
                    await context.Response.WriteAsync("Your number is " + thousandText);
                }
            }
            catch (Exception)
            {
                await context.Response.WriteAsync("Incorrect parameter");
            }
        }
    }
}