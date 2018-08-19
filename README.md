# JWT Provider Service - A Class to parse the JWT Token on server side client , written in C#

Usage:
var  claims = await JWTServices.GetTokenAsync("username", "password").ConfigureAwait(false);
