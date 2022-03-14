namespace JGeneral.IO.RaspberryPi
{
    /// <summary>
    /// Physical board mapping for GPIO pins.
    /// </summary>
    public enum GPIO
    {
        GPIO2 = 3,
        GPIO3 = 5,
        GPIO4 = 7,
        GPIO5 = 29,
        GPIO6 = 31,
        GPIO7 = 26,
        GPIO8 = 24,
        GPIO9 = 21,
        GPIO10 = 19,
        GPIO11 = 23,
        GPIO12 = 32,
        GPIO13 = 33,
        GPIO14 = 8,
        GPIO15 = 10,
        GPIO16 = 36,
        GPIO17 = 11,
        GPIO18 = 12,
        GPIO19 = 25,
        GPIO20 = 38,
        GPIO21 = 40,
        GPIO22 = 15,
        GPIO23 = 16,
        GPIO24 = 18,
        GPIO25 = 22,
        GPIO26 = 37,
        GPIO27 = 13
    }
    /// <summary>
    /// Physical board mapping for I2C pins.
    /// </summary>
    public enum I2C
    {
        SDA1 = 3,
        SCL1 = 5,
        IDSD = 27,
        IDSC = 28
    }
    /// <summary>
    /// Physical board mapping for UART0 pins.
    /// </summary>
    public enum UART0
    {
        TXD = 8,
        RXD = 10
    }
    /// <summary>
    /// Physical board mapping for all power related pins.
    /// </summary>
    public enum Power
    {
        V3_1 = 1,
        V3_2 = 17,
        V5_1 = 2,
        V5_2 = 4,
        GROUND_1 = 6,
        GROUND_2 = 9,
        GROUND_3 = 14,
        GROUND_4 = 20,
        GROUND_5 = 25,
        GROUND_6 = 30,
        GROUND_7 = 34,
        GROUND_8 = 39
    }
}