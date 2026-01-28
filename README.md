# Marketplace.Solution

## How to run application
You can download the file called Publish to your system and run the executable to fire up the api.
The port and url the api listens on will be displayed for usage and accessibility.

## Edge Cases
### Assumptions
1. No external api was used
2. Partial fulfilment scenarios were not catered for - if the total available amount by the vendors isn't enough to fulfil the orders, a response is returned.

### Validations
Input validation was implemented

## Security
Basic API Security was implemented using API Key which is to be passed as a header.


## Requests and Responses
### 1. Allocation
   ```json
{
  "baseCurrency": "USD",
  "quoteCurrency": "EUR",
  "amount": 16000,
  "direction": "BUY"
}
```
