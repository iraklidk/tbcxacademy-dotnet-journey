using System.Text;

namespace ATMApp
{
    static class ExceptionHelper
    {
        public static string GetLastInnerExMessage(Exception ex)
        {
            if (ex is null) return "No Exception";

            if (ex.InnerException is null) return ex.Message;

            return GetLastInnerExMessage(ex.InnerException);
        }

        public static string GetAllInnerExMessage(Exception ex)
        {
            if (ex == null) return "No exception";

            StringBuilder sb = new StringBuilder();
            while(ex is not null)
            {
                sb.Append(ex.Message + ", ");
                ex = ex.InnerException;
            }

            return sb.ToString(0, sb.Length - 2);
        }

        public static void CauseException()
        {
            try
            {
                try
                {
                    // innermost exception
                    throw new Exception("bottom");
                }
                catch (Exception e1)
                {
                    throw new ArgumentNullException("middle", e1);
                }
            }
            catch (Exception e2)
            {
                // outermost
                throw new InvalidOperationException("top", e2);
            }
        }
    }
}