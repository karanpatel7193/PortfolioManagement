
export const CommonPages: any[] = [
    {
        pageTitle: "Profile",
        breadcrumbs: "Profile",
        pageName: "profile",
        leftSearchCriteria: "",
        rightView: "",
        routePath: "/app/profile",
    },
    {
        pageTitle: "Dashboard",
        breadcrumbs: "Dashboard",
        pageName: "dashboard",
        routePath: "/app/dashboard",
    }
];

export const RouteApiData: any[] = [
    {
        pageTitle: "Home",
        breadcrumbs: "Home",
        pageName: "home",
        leftSearchCriteria: "home-menu-tab/",
        rightView: "home-plot/",
        downloadAllReport: "download-all-reports",
        downloadReport: "download-portfolio-summary-report"
    }
]

export const MenuTabsData: any[] = [
    {
        "name": "Dashboard",
        "icon": "bi-house-door",
        "pageName": "dashboard",
        "url": "/app/dashboard",
        "subtabs": [],
        "enabled": true
    },
    {
        "name": "Master",
        "icon": "bi-bounding-box",
        "pageName": "master",
        "url": "/app/master",
        "subtabs": [
            {
                "name": "Script",
                "icon": "",
                "pageName": "script",
                "url": "/app/master-script",
                "subtabs": [],
                "enabled": true
            },
            {
                "name": "Broker",
                "icon": "",
                "pageName": "broker",
                "url": "/app/master-broker",
                "subtabs": [],
                "enabled": true
            },
            {
                "name": "Account",
                "icon": "",
                "pageName": "account",
                "url": "/app/master-account",
                "subtabs": [],
                "enabled": true
            },
            {
                "name": "master",
                "icon": "",
                "pageName": "master",
                "url": "/app/master-master",
                "subtabs": [],
                "enabled": true
            },
        ],
        "enabled": true


    },
    {
        "name": "Account",
        "icon": "bi bi-person-badge-fill",
        "pageName": "account",
        "url": "/app/account",
        "subtabs": [
            {
                "name": "Menu",
                "icon": "",
                "pageName": "menu",
                "url": "/app/account-menu",
                "subtabs": [],
                "enabled": true
            },
            {
                "name": "Roles",
                "icon": "",
                "pageName": "role",
                "url": "/app/account-role",
                "subtabs": [],
                "enabled": true
            },
            {
                "name": "User",
                "icon": "",
                "pageName": "user",
                "url": "/app/account-user",
                "subtabs": [],
                "enabled": true
            }
        ],

        "enabled": true
    },
    {
        "name": "Transaction",
        "icon": "bi bi-credit-card",
        "pageName": "transaction",
        "url": "/app/transaction",
        "subtabs": [
            {
                "name": "StockTransaction",
                "icon": "",
                "pageName": "stocktransaction",
                "url": "/app/transaction-stocktransaction",
                "subtabs": [],
                "enabled": true
            },
        ],
            "enabled": true
    },
    {
        "name": "Reports",
        "icon": "bi-graph-up",
        "pageName": "reports",
        "url": "/app/reports",
        "subtabs": [
            {
                "name": "Stock Transaction Report",
                "icon": "",
                "pageName": "stockTransaction",
                "url": "/app/reports-stockTransaction",
                "subtabs": [],
                "enabled": true
            },
            {
                "name": "Stock Transaction Summary",
                "icon": "",
                "pageName": "stockTranctionSummary",
                "url": "/app/reports-stockTranctionSummary",
                "subtabs": [],
                "enabled": true
            },
            {
                "name": "Porfolio Report",
                "icon": "",
                "pageName": "porfolioReport",
                "url": "/app/reports-portfolioReport",
                "subtabs": [],
                "enabled": true
            }
        ],
        "enabled": true
    },
    {
        "name": "Watchlist",
        "icon": "bi bi-credit-card",
        "pageName": "watchlist",
        "url": "/app/watchlist",
        "subtabs": [
            {
                "name": "Watchlist",
                "icon": "",
                "pageName": "Watchlist",
                "url": "/app/watchlist-watchlist",
                "subtabs": [],
                "enabled": true
            },
        ],
            "enabled": true
    },
    {
        "name": "Analysis",
        "icon": "bi bi-credit-card",
        "pageName": "analysis",
        "url": "/app/analysis",
        "subtabs": [
            {
                "name": "Volume",
                "icon": "",
                "pageName": "Volume",
                "url": "/app/analysis-volume",
                "subtabs": [],
                "enabled": true
            },
        ],
            "enabled": true
    }
    
]
