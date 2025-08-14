namespace RequestProcessingPipeline
{
    public class FromTwentyToHundredMiddleware
    {
        private readonly RequestDelegate _next;

        public FromTwentyToHundredMiddleware(RequestDelegate next)
        {
            this._next = next;
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

                if (number < 20)
                {
                    await _next.Invoke(context);
                    return;
                }
                if (number == 100)
                {
                    string text = "one hundred";
                    if (string.IsNullOrEmpty(context.Session.GetString("remainder_number")))
                    {
                        await context.Response.WriteAsync("Your number is " + text);
                    }
                    else
                    {
                        context.Session.SetString("remainder_text", text);
                    }
                    return;
                }

                string[] Tens = { "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
                string tensText = Tens[number / 10 - 2];

                if (number % 10 != 0 && string.IsNullOrEmpty(context.Session.GetString("remainder_number")))
                {
                    context.Session.SetString("remainder_number", (number % 10).ToString());
                    await _next.Invoke(context);
                    string? result = context.Session.GetString("remainder_text");
                    if (!string.IsNullOrEmpty(result))
                    {
                        tensText += " " + result;
                    }
                }

                if (string.IsNullOrEmpty(context.Session.GetString("remainder_number")))
                {
                    await context.Response.WriteAsync("Your number is " + tensText);
                }
                else
                {
                    context.Session.SetString("remainder_text", tensText);
                }
            }
            catch
            {
                await context.Response.WriteAsync("Incorrect parameter");
            }
        }
    }
}