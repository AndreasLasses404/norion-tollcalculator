# norion-tollcalculator

## Usage

```
[POST]
# /api/Toll
Creates a vehicle of the specified type in the body.
Accepted vehicle types are:
Car,
Diplomat,
Emergency,
Foreign,
Military,
Motorbike,
Tractor

{
 "vehicleType": "string"
}

[PUT]
# /api/Toll/{id]/Passages
The specified vehicle makes a passage through a toll with the current timestamp

[GET]
# /api/Toll/{id}
Retrieves the specified vehicle and calculates the amount to be paid based on previous passages.
Returns an int with the amount to be paid.

```
