(function () {
    angular.module('cbcproject').factory('pagerservice', function () {

        var pages = 100;
        var pageSize = 10;
        var pageStart = 1;
       
        return {
            init:init,
            prevPage:prevPage,
            nextPage:nextPage,
            setPage:setPage
        }
       
        function init (listOfObj) {
            currentIndex = 1;
            pageStart = 1;
            pages = 10;
            filteredItems = listOfObj;
            pageNumber = parseInt("" + (filteredItems.length / pageSize));
            if (filteredItems.length % pageSize != 0) {
                pageNumber++;
            }
            if (pageNumber < pages) {
                pages = pageNumber;
            }
       refreshItems();

            return {
                items: items,
                pagesIndex: pagesIndex,
                pageNumber: pageNumber,
                currentIndex: currentIndex
            };
        };
        // FilterByName() {
        //   filteredItems = [];
        //   if (inputName != "") {
        //     sarData.SarsRefDetails.forEach(element => {
        //       if (element.CompanyNumber.toUpperCase().indexOf(inputName.toUpperCase()) >= 0) {
        //         filteredItems.push(element);
        //       }
        //     });
        //   } else {
        //     filteredItems = sarData.SarsRefDetails;
        //   }
        //   console.log(filteredItems);
        //   init();
        // }
        function fillArray () {
            var obj = new Array();
            for (var index = pageStart; index < pageStart + pages; index++) {
                obj.push(index);
            }
            return obj;
        };
        function refreshItems () {
            items = newMethod().slice((currentIndex - 1) * pageSize, (currentIndex) * pageSize);
            pagesIndex = fillArray();
        };
        function newMethod () {
            return filteredItems;
        };
        function prevPage () {
            if (currentIndex > 1) {
                currentIndex--;
            }
            if (currentIndex < pageStart) {
                pageStart = currentIndex;
            }
          refreshItems();
            return {
                items: items,
                currentIndex: currentIndex,
                pagesIndex: pagesIndex,
                pageNumber: pageNumber
            };
        };
        function nextPage () {
            if (currentIndex < pageNumber) {
                currentIndex++;
            }
            if (currentIndex >= (pageStart + pages)) {
                pageStart = currentIndex - pages + 1;
            }
           refreshItems();
            return {
                items: items,
                currentIndex: currentIndex,
                pagesIndex: pagesIndex,
                pageNumber: pageNumber
            };
        };
        function setPage (index) {
            currentIndex = index;
            refreshItems();
            return {
                items: items,
                currentIndex: currentIndex,
                pagesIndex: pagesIndex,
                pageNumber: pageNumber
            };
        };
    });
})();