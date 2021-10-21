using Microsoft.AspNetCore.Http;

public record UriBase
{
    public UriBase(Uri baseUri)
    {
        Base = baseUri;
    }

    public UriBase(string baseUri) : this(new Uri(baseUri))
    {
    }

    public Uri Base { get; private set; }
    public QueryString QueryString { get; set; }

    public static implicit operator Uri(UriBase baseUri)
        => new Uri(baseUri.ToString());

    public static implicit operator UriBase(string baseUri)
        => new UriBase(baseUri);

    /// <inheritdoc />
    public override string ToString()
        => Base.GetLeftPart(UriPartial.Path) + QueryString.Value;
}
