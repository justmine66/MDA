using System;

namespace MDA.Infrastructure.Utils
{
    public static class ExceptionHelper
    {
        public static void EatException(Action action)
        {
            try
            {
                action();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public static T EatExceptionOrDefault<T>(Func<T> action, T defaultValue = default)
        {
            try
            {
                return action();
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
	}
}
