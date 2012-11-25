import QtQuick 1.1
import "loader.js" as L

Rectangle {
    width: 512
    height: 512

    color: "#400040"

    Rectangle {
        width: 40
        height: 40
        color: "orange"
        x: 40
        y: 40
    }

    MegaCat {
        anchors.fill: parent
    }
}
