# Stock Market Portfolio Management System

## Overview
The Stock Market Portfolio Management System is a comprehensive application designed to help users manage their stock investments. Built with .NET on the backend and Angular on the frontend, this application allows users to track their stock portfolios, monitor performance, and make informed investment decisions.

## Features
- **User Authentication**: Secure login and registration using JWT.
- **User Management**: Create, update, and delete users for managing portfolio.
- **Real-time Stock Data**: Integration with external APIs to fetch real-time stock prices.
- **Performance Analytics**: Visualize portfolio performance with charts and graphs.
- **Transaction History**: Maintain a history of all buy/sell transactions.
- **Watchlist**: Set up watchlist for stock price changes.

## Technologies Used
- **Backend**: .NET 6.0
- **Frontend**: Angular 14.2.0
- **Database**: SQL Server
- **Authentication**: JSON Web Tokens (JWT)
- **APIs**: Integration with external stock market APIs

## Installation

### Prerequisites
- .NET Core SDK 3.1
- Node.js and npm
- SQL Server

### Backend Setup
1. Clone the repository:
    ```bash
    git clone https://github.com/yourusername/stock-portfolio-management.git
    ```
2. Open project in the Visual studio and Restore the .NET Core dependencies:
3. Update the `appsettings.json` with your SQL Server connection string and other configurations.
4. Run the application

### Frontend Setup
1. Navigate to the frontend directory:
    ```bash
    cd PortfolioManagement_UI
    ```
2. Install the Angular dependencies:
    ```bash
    npm install
    ```
3. Update the `environment.ts` file with the backend API URL.
4. Run the Angular application:
    ```bash
    ng serve -o
    ```

## Usage
1. Navigate to `http://localhost:4400` in your web browser.
2. Register a new account or log in with an existing account.
3. Start adding your stock stransactions.
4. Monitor your portfolio performance and view detailed analytics.

## Contributing
Contributions are welcome! Please fork the repository and submit a pull request with your changes.

## License
This project is licensed under the MIT License.

## Contact
For any questions or suggestions, please contact [karan.patel7193@gmail.com](mailto:karan.patel7193@gmail.com).

---

Feel free to modify this template according to your project's specific requirements and details. If you need any more specific information included, just let me know! 📈💼🚀
