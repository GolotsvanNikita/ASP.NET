namespace RequestProcessingPipeline
{
    public static class FromThousandToTenthousandExtensions
    {
        public static IApplicationBuilder UseFromThousandToTenthousand(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FromThousandToTenthousandMiddleware>();
        }
    }
}
