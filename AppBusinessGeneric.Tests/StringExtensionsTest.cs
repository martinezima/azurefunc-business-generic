using AppBusinessGeneric.Application.Utilities;
using Armanino.Integration.Utilities.Models;

namespace AppBusinessGeneric.Tests;


public class StringExtensionsTest
{
    [Fact]
    public void ConverToType_SetStringValueTo125_2030_ReturnDouble()
    {
        // Arrange
        var value = "125.2030";

        // Action
        var result = StringExtensions.ConverToType(value);

        // Assert
        Assert.Equal(TypeCode.Double, Type.GetTypeCode(result.GetType()));
    }
    [Fact]
    public void ConverToType_SetStringValueToRyan_ReturnString()
    {
        // Arrange
        var value = "Ryan";

        // Action
        var result = StringExtensions.ConverToType(value);

        // Assert
        Assert.Equal(TypeCode.String, Type.GetTypeCode(result.GetType()));
    }
    [Fact]
    public void ConverToType_SetStringValueTo125_ReturnInt16()
    {
        // Arrange
        var value = "125";

        // Action
        var result = StringExtensions.ConverToType(value);

        // Assert
        Assert.Equal(TypeCode.Int16, Type.GetTypeCode(result.GetType()));
    }
    [Fact]
    public void ConverToType_SetStringValueToTrue_ReturnBool()
    {
        // Arrange
        var value = "true";

        // Action
        var result = StringExtensions.ConverToType(value);

        // Assert
        Assert.Equal(TypeCode.Boolean, Type.GetTypeCode(result.GetType()));
    }
}