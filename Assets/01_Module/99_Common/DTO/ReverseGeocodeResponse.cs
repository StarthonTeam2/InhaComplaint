using System;

[Serializable]
public class ReverseGeocodeResponse
{
    public ReverseGeocodeResult[] results;

    public override string ToString()
    {
        string city = "";
        string district = "";
        string dong = "";

        foreach (var result in results)
        {
            if (result.name == "admcode")
            {
                city = result.region.area1.name;
                district = result.region.area2.name;
                dong = result.region.area3.name;
            }
            else if (result.name == "roadaddr" && result.land != null)
            {
                string address = $"{result.land.name} {result.land.number1}";

                if (!string.IsNullOrEmpty(result.land.number2))
                    address += $"-{result.land.number2}";

                return $"{city} {district} {address}".Trim();
            }
        }

        return $"{city} {district} {dong}".Trim();
    }
}

[Serializable]
public class ReverseGeocodeResult
{
    public string name;
    public ReverseGeocodeRegion region;
    public ReverseGeocodeLand land;
}

[Serializable]
public class ReverseGeocodeRegion
{
    public ReverseGeocodeArea area1;
    public ReverseGeocodeArea area2;
    public ReverseGeocodeArea area3;
}

[Serializable]
public class ReverseGeocodeArea
{
    public string name;
}

[Serializable]
public class ReverseGeocodeLand
{
    public string name;
    public string number1;
    public string number2;
}