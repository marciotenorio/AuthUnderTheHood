# Security in ASP NET

# Installation
To https works: ``dotnet dev-certs https --trust``

# Introduction
![](./img/auth-flow.png)

In every request the security context is verified (authenticated) by middlewares, filter, etc.
Cookies are used mainly for session management, personalization and tracking.

## Security Context
![](./img/sc-hierarchy.png)

All security-related information/data that is relevant to security like: name, user identification, roles, etc.

## Authentication
![](./img/authentication-implementation.png)

Verify if you are who you say you are, implies to generate security context.

## Authorization
Verify if security context satisfies the access requirements.

## Cookie vs Token
[Article here...](https://auth0.com/blog/cookies-tokens-jwt-the-aspnet-core-identity-dilemma/#Wait--Identity-API-Endpoints-Have-Cookies-Too-)
- Cookies are tied to a domain, require little storage, and are automatically managed by the browser. However, 
without proper precautions, they are vulnerable to CSRF and XSS attacks, are not suitable for multi-domain applications, and have scalability issues.
- Tokens are not tied to only web applications and they promote server scalability. However, their management and 
security is left to the developer. Depending on the type of token, there may be an invalidation problem, i.e. a revoked 
token may still be used by the client, and a data transfer overhead if the token size becomes significant.


# ASP NET Core Security Basics

The middleware follow the chain-of-responsabilities pattern, to deal with all things in the pipeline 
— authentication, authorization, other handlers.

The main concept in ASP NET is the **ClaimsPrincipal** representing the security context, that can have one or more
identities (having a default one). If not logged, identity is anonymous.

Browser session can affect the cookie lifetime (how the session is managed is specific per browser implementation). The cookie can expire together with browser session or lasts more (persistent cookie).\
====Browser Lifetime===== | == Browser Lifetime ==             
=================== Cookie Lifetime ==========================


## Authentication
The middleware will check if has the cookie, is valid, descrypt and populate the security context.

## Authorization
![](./img/authorization-flow.png)

When some action needs to be performed in specific pages the **IAuthorizationService** will find all
requirement handlers that deal with those requirement of that action. 




