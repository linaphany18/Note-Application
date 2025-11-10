export class filterModel {
    searchTerm?: string;
    orderBy?: string;
    
    constructor(
        searchTerm?: string,
        orderBy?: string
    ) {
        this.searchTerm = searchTerm;
        this.orderBy = orderBy;
    }
}