using System.Xml;
using Microsoft.IdentityModel.Tokens;

namespace Web_API.Service;

public class CustomOldTokenHandler : SecurityTokenHandler
{
    public override Type TokenType => throw new NotImplementedException();

    public override SecurityToken ReadToken(XmlReader reader, TokenValidationParameters validationParameters)
    {
        throw new NotImplementedException();
    }

    public override void WriteToken(XmlWriter writer, SecurityToken token)
    {
        throw new NotImplementedException();
    }
}