import { Component, OnInit } from '@angular/core';
import { CustomerService } from '../_services/customer.service';
import { AuthenticationService } from '../_services';
import * as XLSX from 'xlsx';

@Component({
  selector: 'app-get-reports',
  templateUrl: './get-reports.component.html',
  styleUrls: ['./get-reports.component.css']
})
export class GetReportsComponent implements OnInit {
  reportData: any;
  searchText;
  constructor(private customerService: CustomerService,
    private authenticationService: AuthenticationService,
    ) { }

  ngOnInit() {
    this.getScoreReports()
  }

  getScoreReports() {
    var userVal = this.authenticationService.currentUserValue;
    if (userVal != null && userVal.id != null)
      this.customerService.getUserReports(userVal.id).subscribe(
      data => {
        this.reportData = data;
        return data;
      });
  }

    /*name of the excel-file which will be downloaded. */
    fileName = 'ExcelSheet.xlsx';

    exportexcel(): void {
      /* table id is passed over here */
      let element = document.getElementById('credit-table');
      const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(element);
  
      /* generate workbook and add the worksheet */
      const wb: XLSX.WorkBook = XLSX.utils.book_new();
      XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');
  
      /* save to file */
      XLSX.writeFile(wb, this.fileName);
  
    }
  
  

}
