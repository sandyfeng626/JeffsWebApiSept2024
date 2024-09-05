# Issues API Security Requirements

## Needs Authentication for All Use
We won't have a "public" API - every request must contain a valid identity in the form of a JSON Web Token (JWT) from a valid issuer we trust.

## Vendors

Any authenticated user can `GET` the vendor list.

- Required Authorizaton
- We are using Authentication and Authorization services from .NET, and added the JwtBearer package and config for that.

`Microsoft.AspNetCore.Authentication.JwtBearer` package.


Authenticated users that are SoftwareCenterAdmin and SoftwareCenter can add new Vendors to the list.

Only the user the that created a vendor can delete that vendor (along with the rules above)

## Software

Any Authenticated user can add an issue for a piece of software that exists.

