namespace RequestProcessingPipeline
{
    public class FromOneToTenMiddleware
    {
        private readonly RequestDelegate _next;

        public FromOneToTenMiddleware(RequestDelegate next)
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

                if (number > 10)
                {
                    await _next.Invoke(context);
                    return;
                }

                string text;
                if (number == 10)
                {
                    text = "ten";
                }
                else
                {
                    string[] Ones = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
                    text = Ones[number - 1];
                }

                if (string.IsNullOrEmpty(context.Session.GetString("remainder_number")))
                {
                    await context.Response.WriteAsync("Your number is " + text);
                }
                else
                {
                    context.Session.SetString("remainder_text", text);
                }
            }
            catch
            {
                await context.Response.WriteAsync("Incorrect parameter");
            }
        }
    }
}