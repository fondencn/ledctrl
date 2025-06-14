using System.Drawing;

namespace LedCtrl
{
    public interface ILEDService
    {
        /// <summary>
        /// Sets all LEDs to a single color.
        /// </summary>
        /// <param name="color">The color to set.</param>
        void SetSolidColor(Color color);

        /// <summary>
        /// Sets all LEDs to individual colors.
        /// </summary>
        /// <param name="colors">Array of colors, must match LED count.</param>
        void SetLeds(Color[] colors);

        /// <summary>
        /// Spins the LEDs with a specified color and count.
        /// </summary>
        /// <param name="color">The color to spin.</param>
        /// <param name="count">The number of times to spin.</param>
        void Spin(Color color, int count = 1);

        /// <summary>
        /// Sets the LEDs to trace mode.
        /// </summary>
        void Trace();

        /// <summary>
        /// Sets the LEDs to listen mode.
        /// </summary>
        void Listen();

        /// <summary>
        /// Sets the LEDs to wait mode.
        /// </summary>
        void Wait();

        /// <summary>
        /// Sets the LEDs to speak mode.
        /// </summary>
        void Speak();
    }
}