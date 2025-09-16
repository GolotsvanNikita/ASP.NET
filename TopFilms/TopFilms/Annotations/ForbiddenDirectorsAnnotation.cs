using System.ComponentModel.DataAnnotations;

namespace TopFilms.Annotations
{
    public class ForbiddenDirectorsAnnotation : ValidationAttribute
    {
        private static string[] forbiddenDirectors;

        public ForbiddenDirectorsAnnotation(string[] Directors)
        {
            forbiddenDirectors = Directors;
        }

        public override bool IsValid(object? value)
        {
            if (value != null)
            {
                string? strval = value.ToString();

                for (int i = 0; i < forbiddenDirectors.Length; i++)
                {
                    if (strval == forbiddenDirectors[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
