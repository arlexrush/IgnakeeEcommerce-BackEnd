using Ecommerce.Domain;

namespace Ecommerce.UnitTests;

public class ShippingTests
{
    /// <summary>
    /// Prueba unitaria para verificar que el método SetShippingCost almacena correctamente un costo de envío no negativo.
    /// </summary>
    [Fact]
    public void SetShippingCostStoresNonNegativeCost()
    {
        var shipping = new Shipping();

        shipping.SetShippingCost(7.50m);

        Assert.Equal(7.50m, shipping.TotalShipping);
    }

    /// <summary>
    /// Prueba unitaria para verificar que el método SetShippingCost lanza una excepción ArgumentOutOfRangeException cuando se intenta establecer un costo de envío negativo.
    /// </summary>
    [Fact]
    public void SetShippingCostRejectsNegativeCost()
    {
        var shipping = new Shipping();

        Assert.Throws<ArgumentOutOfRangeException>(() => shipping.SetShippingCost(-1m));
    }

    /// <summary>
    /// Prueba unitaria para verificar que el método IsReadyForFulfillment requiere tanto un operador de envío asignado como un costo de envío establecido para devolver true.
    /// </summary>
    [Fact]
    public void IsReadyForFulfillmentRequiresOperatorAndCost()
    {
        var shipping = new Shipping();
        shipping.SetShippingCost(7.50m);

        var readyWithoutOperator = shipping.IsReadyForFulfillment();
        shipping.AssignOperator(new ShippingOperator());
        var readyWithOperator = shipping.IsReadyForFulfillment();

        Assert.False(readyWithoutOperator);
        Assert.True(readyWithOperator);
    }
}
