import {
    Chart
} from "chart.js/dist/chart.js";

// Simplifies the creation of chart by providing a single function interface
export function CreateChart(canvasId, chartType, datasets) {
    let canvasItem = $(`#${canvasId}`);
    const config = {
        type: chartType,
        data: {
            datasets: datasets
        },
        options: {
            onHover: function (event, activeElements) {
                if (canvasItem.hasClass("chartutil-pointer")) {
                    canvasItem.css("cursor", activeElements[0] ? "pointer" : "default");
                }
            },
            plugins: {
                tooltip: {
                    callbacks: {
                        beforeLabel: function (item) {
                            const datasetItem = item.dataset.data[item.dataIndex];
                            if (datasetItem && datasetItem.tooltip) {
                                return datasetItem.tooltip;
                            }
                        }
                    }
                }
            },
            parsing: {
                xAxisKey: 'label',
                yAxisKey: 'value'
            }
        }
    };

    if (!canvasItem) {
        throw `Canvas item wasn't found! Used selector: ${chartSelector}`;
    }

    let chart = new Chart(
        canvasItem[0],
        config
    );

    console.log(chart);

    canvasItem.trigger(initializeGraphEventType, [chart]);
    return chart;
}

// To add events to canvas in separate view is to wait for it to initialize. With these two methods this can be done safely
const initializeGraphEventType = "initialize.chartutil";

// Either already initialized or will eventually. 
// Duplicate initializations shouldn"t happen, but if they will then most likely this action must repeat
export function ListenInitializationEvent(canvasId, action) {
    const chart = Chart.getChart(canvasId);
    if (chart) {
        console.log("FoundGraphInitialized");
        action(chart);
    }

    const canvas = $(`#${canvasId}`);
    if (canvas.length === 0) {
        throw "Can't find element to bind the listening event to!";
    }

    canvas.on(initializeGraphEventType, function (event, data) {
        // TODO: remove log
        console.log("FiredInitEvent");
        action(data);
    });
}

export function AddEvent(chart, eventType, eventHandler) {
    $(chart.canvas).on(eventType, function (event) {
        const affectedElements = chart.getElementsAtEventForMode(event, "nearest", { intersect: true }, true);
        eventHandler(event, affectedElements);
    }.bind(this));
}