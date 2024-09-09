using System.Collections;
using AppBusinessGeneric.Application.Helpers;
using AppBusinessGeneric.Application.Models;
using Armanino.Integration.Utilities.Models;

namespace AppBusinessGeneric.Tests;


public class ConverterObjectHelperTest
{
    [Fact]
    public void GetTypeBySystemObject_SendArgumentWithValidType_ReturnNotNull()
    {
        // Arrange
        var systemObject = "SupplierConfigurationImportBatches";

        // Action
        var result = ConverterObjectHelper.GetTypeBySystemObject(systemObject);

        // Assert
        Assert.NotNull(result);
    }
    [Fact]
    public void GetTypeBySystemObject_SendArgumentWithInvalidType_ThrowException()
    {
        // Arrange
        var systemObject = "InvalidModel";

        // Action && Assertion
        var exception = Assert.Throws<NullReferenceException>(
            () => ConverterObjectHelper.GetTypeBySystemObject(systemObject));
        Assert.Equal($"The type {systemObject} does not exist.", exception.Message);  
    }
    [Fact]
    public void CheckIfIsAList_SetFieldsWithElementsClassTypeValidAndSystemObject_RetunTrue()
    {
        // Arrange
        var systemObject = "SupplierConfigurationImportBatches";
        var fields = new List<Field>
        {
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[0].ErpSourceId",
                    Value = "Erp Source Id 1"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[0].Company",
                    Value = "Company Test 1"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[0].FirstName",
                    Value = "Miguel"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[0].LastName",
                    Value = "Martinez"
                }
            },
        };

        // Action
        var isAList = ConverterObjectHelper.CheckIfIsAList(
            fields,
            systemObject);
        
        // Assertion
        Assert.True(isAList);
    }
    [Fact]
    public void CheckIfIsAList_SetFieldsWithElementsClassTypeValidAndSystemObject_RetunFalse()
    {
        // Arrange
        var systemObject = "SupplierConfigurationImportBatches";
        var fields = new List<Field>
        {
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches.ErpSourceId",
                    Value = "Erp Source Id 1"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches.Company",
                    Value = "Company Test 1"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches.FirstName",
                    Value = "Miguel"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches.LastName",
                    Value = "Martinez"
                }
            },
        };

        // Action
        var isAList = ConverterObjectHelper.CheckIfIsAList(
            fields,
            systemObject);
        
        // Assertion
        Assert.False(isAList);
    }
    [Fact]
    public void CheckOverAllProperties_SetFieldsValidInstanceOfSupplierConfigurationImportBatchesListType_ReturnListWithOneItem()
    {
        // Arrange
        var systemObject = "SupplierConfigurationImportBatches";
        var assemblyName =
        $"{ConverterObjectHelper.MODEL_NAMESPACE_FOR_GENERIC}" +
        $".{systemObject}, AppBusinessGeneric.Application" +
        $", Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        var classType = Type.GetType(assemblyName);
        Type genericListType = typeof(List<>);
        Type concreteListType = genericListType.MakeGenericType(
            classType ?? typeof(object));
        dynamic instance = Activator.CreateInstance(concreteListType) ?? new object();
        var fields = new List<Field>
        {
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[0].ErpSourceId",
                    Value = "Erp Source Id 1"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[0].Company",
                    Value = "Company Test 1"
                }
            }
        };

        // Action
        var expectedResult = ConverterObjectHelper.CheckOverAllProperties(
            fields,
            instance,
            classType,
            systemObject,
            true);
        
        // Assertion
        Assert.Single(expectedResult);
        Assert.Equal(expectedResult[0].ErpSourceId, "Erp Source Id 1");
        Assert.Equal(expectedResult[0].Company, "Company Test 1");
    }
    [Fact]
    public void CheckOverAllProperties_SetFieldsValidInstanceOfSupplierConfigurationImportBatchesListType_ReturnListWithTwoItems()
    {
        // Arrange
        var systemObject = "SupplierConfigurationImportBatches";
        var assemblyName =
        $"{ConverterObjectHelper.MODEL_NAMESPACE_FOR_GENERIC}" +
        $".{systemObject}, AppBusinessGeneric.Application" +
        $", Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        var classType = Type.GetType(assemblyName);
        Type genericListType = typeof(List<>);
        Type concreteListType = genericListType.MakeGenericType(
            classType ?? typeof(object));
        dynamic instance = Activator.CreateInstance(concreteListType) ?? new object();
        var fields = new List<Field>
        {
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[0].ErpSourceId",
                    Value = "Erp Source Id 1"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[0].Company",
                    Value = "Company Test 1"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[1].ErpSourceId",
                    Value = "Erp Source Id 2"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[1].Company",
                    Value = "Company Test 2"
                }
            }
        };

        // Action
        var expectedResult = ConverterObjectHelper.CheckOverAllProperties(
            fields,
            instance,
            classType,
            systemObject,
            true);
        
        // Assertion
        Assert.NotNull(expectedResult);
        Assert.Collection(expectedResult as List<SupplierConfigurationImportBatches> ?? [],
            e =>
            {
                Assert.Equal("Erp Source Id 1", e.ErpSourceId);
                Assert.Equal("Company Test 1", e.Company);
            },
            e =>
            {
                Assert.Equal("Erp Source Id 2", e.ErpSourceId);
                Assert.Equal("Company Test 2", e.Company);
            }
        );
    }
    [Fact]
    public void CheckOverAllProperties_SetFieldsValidInstanceOfSupplierConfigurationImportBatchesListTypeAndListOfTProperty_ReturnListWithOneItem()
    {
        // Arrange
        var systemObject = "SupplierConfigurationImportBatches";
        var assemblyName =
        $"{ConverterObjectHelper.MODEL_NAMESPACE_FOR_GENERIC}" +
        $".{systemObject}, AppBusinessGeneric.Application" +
        $", Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        var classType = Type.GetType(assemblyName);
        Type genericListType = typeof(List<>);
        Type concreteListType = genericListType.MakeGenericType(
            classType ?? typeof(object));
        dynamic instance = Activator.CreateInstance(concreteListType) ?? new object();
        var fields = new List<Field>
        {
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[0].ErpSourceId",
                    Value = "Erp Source Id 1"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[0].Company",
                    Value = "Company Test 1"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[0].ContactPersons[0].Country",
                    Value = "United States"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[0].ContactPersons[0].FirstName",
                    Value = "Miguel"
                }
            },
            {
                new Field
                {
                    Name = "SupplierConfigurationImportBatches[0].ContactPersons[0].LastName",
                    Value = "Martinez"
                }
            },
        };

        // Action
        var expectedResult = ConverterObjectHelper.CheckOverAllProperties(
            fields,
            instance,
            classType,
            systemObject,
            true);
        
        // Assertion
        Assert.Single(expectedResult);
        Assert.Equal(expectedResult[0].ContactPersons[0].Country, "United States");
        Assert.Equal(expectedResult[0].ContactPersons[0].FirstName, "Miguel");
        Assert.Equal(expectedResult[0].ContactPersons[0].LastName, "Martinez");
    }

}