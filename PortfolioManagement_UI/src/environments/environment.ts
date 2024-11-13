// This file can be replaced during build by using the `fileReplacements` array.
// `ng build` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
    production: false,
	apiUrl: 'https://localhost:7143/',
	downloadUrl: 'https://localhost:7143/',
    softwareVersion: '1.0',
    isOurScriptPage:false,
	ourScriptPageUrl:'http://localhost:4400/app/scriptView/',
	otherScriptPageUrl:'https://www.nseindia.com/get-quotes/equity?symbol='
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/plugins/zone-error';  // Included with Angular CLI.
