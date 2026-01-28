# Marketplace.Solution

## How to run application
You can download the file called Publish to your system and run the executable to launch the api.
Open the appSettings file and change the path of DefaultConnection to the 
The port and url the api listens on will be displayed for usage and accessibility.

## Build Link <a id='Build'>[https://drive.google.com/file/d/1zJbmiOHcT0GvvbbIlyOwe5JQCs8T2jOd/view?usp=drive_link](https://drive.google.com/drive/folders/1gXaH2h-WXmc_WveZVSsUtNHyEBIuRFQx?usp=sharing)</a>

## Edge Cases
### Assumptions
1. No external api was used
2. Partial fulfilment scenarios were not catered for - if the total available amount by the vendors isn't enough to fulfil the orders, a response is returned.

### Validations
Input validation was implemented

## Security
Basic API Security was implemented using API Key which is to be passed as a header.


## Requests and Responses
#### 1. Allocation Request
   ```json
{ 
    "vendorId": "VENDOR_H", 
    "baseCurrency": "USD", 
    "quoteCurrency": "GBP", 
    "rate": 1.29, 
    "available": 5500 
}
```
   
   #### 2. Allocation Response (success)
   ```json
{
    "data": {
        "id": "ab22d5f7-0c8c-42cb-ab15-80b013b2f9bd"
    },
    "responseCode": "00",
    "responseMessage": "Allocation added successfully"
}
```

   #### 3. Order Request (BUY)
```json
{
  "baseCurrency": "USD",
  "quoteCurrency": "EUR",
  "amount": 16000,
  "direction": "BUY"
}
```

   #### 4. Order Response (success)
   This includes the breakdown on how the final rate for the fulfilment was arrived at. This is to provide a bit of transparency into the processing flow.
```json
{
    "data": {
        "orderId": "ed29040d-43ed-4d55-9536-c692a3f3b323",
        "status": "FILLED",
        "requestDetails": {
            "baseCurrency": "USD",
            "quoteCurrency": "EUR",
            "amount": 1000,
            "direction": "BUY"
        },
        "executionDetails": {
            "weightedAverageRate": 1.28,
            "totalCostQuoteCurrency": 1280.00,
            "breakdown": [
                {
                    "vendorId": "VENDOR_C",
                    "amountFilled": 1000,
                    "rate": 1.28,
                    "cost": 1280.00
                }
            ]
        }
    },
    "responseCode": "00",
    "responseMessage": "Order processed successfully"
}
```






   
