﻿import Vue from "vue";
import VideoBar from "./VueComponents/VideoBar";
import MembersList from "./VueComponents/MembersList";

$(".videos-app").each(function () {
    const widgetId = $(this).data("id");
    const videoApp = new Vue({
        el: `#videos-app-${widgetId}`,
        data: {
            videos: videos[widgetId]
        },
        template: "<video-bar :videos='videos' />",
        components: { VideoBar }
    });
});

if ($("#search-widget").length > 0) {
    const searchApp = new Vue({
        el: `#search-widget`,
        data: {
            members: memberList,
            instruments: instruments
        },
        template: "<members-list :members='members' :instruments='instruments' />",
        components: { MembersList }
    });
}