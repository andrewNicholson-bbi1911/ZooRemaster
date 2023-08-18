public static class ProductTypeFinder
{
    public static int FindProductType(ProductType productType)
    {
        if (productType is ProductType0)
        {
            return 0;
        }
        else if (productType is ProductType1)
        {
            return 1;
        }
        else if (productType is ProductType2)
        {
            return 2;
        }
        else if (productType is ProductType3)
        {
            return 3;
        }
        else if (productType is ProductType4)
        {
            return 4;
        }

        return -1;
    }
}
