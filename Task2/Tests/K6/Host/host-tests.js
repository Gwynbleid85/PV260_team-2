import { group } from 'k6';

import ReportsAllTests from '../Reports/reports-tests.js';
// thresholds will decide wether tests pass or not in pre-push hook!
/*export const options = {
    thresholds: {
        'http_req_duration': ['p(95) < 200'],
    },
}*/

// in terminal run "k6 run <path-to-script>"

export default function () {
    group('All Tests', function () 
    {
        ReportsAllTests();
    })
}