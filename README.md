# Redis Cart

Microservice used for storing the user carts for ecommerce websites. Provides generic functionality that can be scaled up depending on the system requirements. At the moment it can fetch the user cart, add/remove items to cart and change the quantity of the products in cart.

To run locally, use the commands: 
###### docker run --name redis_cart -p 5302:6379 -d redis
###### dotnet run

To run inside Docker, run the commands:
###### docker build -t redis_cart_web_api .
###### docker-compose up

To see a list of available endpoints, access the Swagger doc at http://localhost:5300/swagger/index.html. Any URL in the Doc has to be appended to the service's address (http://localhost:5300).
