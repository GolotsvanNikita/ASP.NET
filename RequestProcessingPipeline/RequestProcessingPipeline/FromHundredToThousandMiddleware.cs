namespace RequestProcessingPipeline
{
    public class FromHundredToThousandMiddleware
    {
        private readonly RequestDelegate _next;

        public FromHundredToThousandMiddleware(RequestDelegate next)
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

                if (number < 100)
                {
                    await _next.Invoke(context);
                    return;
                }
                if (number == 1000)
                {
                    string text = "one thousand";
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

                string[] Ones = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
                string[] ElevenNineteen = { "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                string[] Tens = { "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                int numHundred = number / 100;
                int center = number % 100;
                string hundredText;

                if (center == 0)
                {
                    hundredText = Ones[numHundred - 1] + " hundred";
                }
                else if (center >= 11 && center <= 19)
                {
                    hundredText = Ones[numHundred - 1] + " hundred " + ElevenNineteen[center - 11];
                }
                else if (center < 10)
                {
                    hundredText = Ones[numHundred - 1] + " hundred " + Ones[center - 1];
                }
                else if (center == 10)
                {
                    hundredText = Ones[numHundred - 1] + " hundred ten";
                }
                else if (center % 10 == 0)
                {
                    hundredText = Ones[numHundred - 1] + " hundred " + Tens[center / 10 - 2];
                }
                else
                {
                    hundredText = Ones[numHundred - 1] + " hundred " + Tens[center / 10 - 2] + " " + Ones[center % 10 - 1];
                }

                if (string.IsNullOrEmpty(context.Session.GetString("remainder_number")))
                {
                    await context.Response.WriteAsync("Your number is " + hundredText);
                }
                else
                {
                    context.Session.SetString("remainder_text", hundredText);
                }
            }
            catch
            {
                await context.Response.WriteAsync("Incorrect parameter");
            }
        }
    }
}