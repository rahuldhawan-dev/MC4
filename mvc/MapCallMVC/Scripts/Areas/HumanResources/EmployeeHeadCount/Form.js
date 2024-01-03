(($) => {
    const ELEMENTS = {
        unionCount: $('#UnionCount'),
        nonUnionCount: $('#NonUnionCount'),
        otherCount: $('#OtherCount'),
        totalCount: $('#TotalCount')
    }
    const methods = {
        init: () => {
            ELEMENTS.unionCount.on('input', methods.updateCount);
            ELEMENTS.nonUnionCount.on('input', methods.updateCount);
            ELEMENTS.otherCount.on('input', methods.updateCount);
            methods.updateCount();
        },
        updateCount: () => {
            const getInt = (element) => {
                const parsedInt = parseInt(element.val());
                return Number.isNaN(parsedInt) ? 0 : parsedInt;
            }

            const totalCount = getInt(ELEMENTS.unionCount) +
                               getInt(ELEMENTS.nonUnionCount) +
                               getInt(ELEMENTS.otherCount);
            ELEMENTS.totalCount.html(totalCount);
        }
    };

    $(methods.init);
})(jQuery);