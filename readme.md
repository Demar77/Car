## Application Requirements

### Overview
Refine and enhance a .NET Core-based backend application for booking tire changes. Your task is to ensure the application not only functions correctly but also adheres to professional standards of reliability, maintainability, and transparency in operation.

### Functional Requirements

1. **Pricing Service**:
   - Implement a dynamic pricing strategy based on:
     - Car Type (e.g., Sedan: $100, SUV: $120, Truck: $150, Others: $90)
     - Tire Size (e.g., ≤16": $20, 17"-18": $40, >18": $60 surcharge)
     - Optional Services (e.g., Wheel Alignment: additional $50)
   - The service should accurately calculate the total cost considering these factors.

2. **Booking Service**:
   - Process and validate booking requests.
   - Ensure bookings align with slot availability.
   - Provide detailed and structured booking confirmations.

3. **Slot Service**:
   - Efficiently manage and retrieve available slots.
   - Ensure the slot data is current and accurately reflects availability.

4. **General Application Behavior**:
   - The application must reliably handle user input and operational scenarios.
   - It should be evident from the application's output what operations it is performing and how it is arriving at its results.

### Non-Functional Requirements

1. **Code Quality and Structure**:
   - Code should be well-organized, readable, and adhere to standard conventions.
   - Implement patterns and practices that support maintainability and scalability.

2. **Performance and Scalability**:
   - Optimize for efficiency and performance under varying loads.
   - Design should accommodate growing user demand and evolving requirements.

3. **Security and Safety**:
   - Safeguard against common vulnerabilities and ensure data integrity.

4. **Understanding and Transparency**:
   - The application's processes and decisions should be discernible and traceable.

5. **Verification and Assurance**:
   - It is crucial to demonstrate that the application meets its requirements.

### Additional Notes
- The existing solution builds but does not fully meet these requirements.
- Enhancements, refactoring, and additional implementations are expected.
- Documenting your approach and choices will add value to your submission.
